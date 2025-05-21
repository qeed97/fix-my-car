import React from 'react';
import {toast} from "react-hot-toast";
import {useNavigate} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowDown, faArrowUp} from "@fortawesome/free-solid-svg-icons"

export default function FixComponent({fix,problem,setFixes,setProblemData, user, cookies, setUser}) {
    const showErrorToast = (message) => toast.error(message);
    const navigate = useNavigate();
    //const deleteFix = async () => {}

        //formatTimeDifference,
        //handleAccept,

    const sendUpvote = async (id, cookies) => {
        const res = await fetch('/api/Fix/' + id + '/upvote', {
            method: 'PATCH',
            headers: {
                Authorization: "Bearer " + cookies.user
            }
        })
        let data;
        data = await res.json();
        if (data.message) {
            showErrorToast(data.message);
            throw new Error();
        }
    }

    const sendDownvote = async (id, cookies) => {
        const res = await fetch('/api/Fix/' + id + '/downvote', {
            method: 'PATCH',
            headers: {
                Authorization: "Bearer " + cookies.user
            }
        })
        let data;
        data = await res.json();
        if (data.message) {
            showErrorToast(data.message);
            throw new Error();
        }
    }

    const checkIfUpvoted = (id, user) => {
        return user.upvotes.some(identifier => identifier === id);
    }

    const checkIfDownvoted = (id, user) => {
        return user.downvotes.some(identifier => identifier === id);
    }

    const handleUpvoting = async (fix, user, cookies, showErrorToast, setFixes, setUser) => {
        if (checkIfUpvoted(fix.id, user)) {
            try {
                await sendUpvote(fix.id, cookies);
            } catch (e) {
                console.log(e);
                return;
            }
            setFixes(prevFixes =>
                prevFixes.map(f =>
                    f.id === fix.id ? {...f, votes: f.votes - 1} : f
                ).sort((a, b) => b.votes - a.votes)
            );
            setUser(prevUser => ({
                ...prevUser,
                upvotes: prevUser.upvotes.filter(fixId => fixId !== fix.id)
            }));
        } else {
            try {
                await sendUpvote(fix.id, cookies);
            } catch (e) {
                console.log(e);
                return;
            }
            setFixes(prevFixes =>
                prevFixes.map(f =>
                    f.id ===fix.id ? {
                        ...f,
                        votes: checkIfDownvoted(fix.id, user) ? f.votes + 2 : f.votes + 1
                    } : f
                ).sort((a, b) => b.votes - a.votes)
            );

            setUser(prevUser => ({
                ...prevUser,
                upvotes: [...prevUser.upvotes, fix.id],
                downvotes: prevUser.downvotes.filter(ide => ide !== fix.id)
            }));
        }
    };

    const handleDownvoting = async (fix, user, cookies, showErrorToast, setFixes, setUser) => {
        if (checkIfDownvoted(fix.id, user)) {
            try {
                await sendDownvote(fix.id, cookies);
            } catch (e) {
                console.log(e);
                return;
            }

            setFixes(prevFixes =>
                prevFixes.map(f =>
                    f.id === fix.id ? {...f, votes: f.votes + 1 } : f
                ).sort((a, b) => b.votes - a.votes)
            );

            setUser(prevUser => ({
                ...prevUser,
                downvotes: prevUser.downvotes.filter(fixId => fixId !== fix.id)
            }));
        } else {
            try {
                await sendDownvote(fix.id, cookies);
            } catch (e) {
                console.log(e);
                return;
            }

            setFixes(prevFixes =>
                prevFixes.map(f =>
                    f.id === fix.id ? {
                        ...f,
                        votes: checkIfUpvoted(fix.id, user) ? f.votes - 2 : f.votes - 1
                        } : f
                        ).sort((a, b) => b.votes - a.votes)
            );

            setUser(prevUser => ({
                ...prevUser,
                downvotes: [...prevUser.downvotes, fix.id],
                upvotes: prevUser.upvotes.filter(ide => ide !== fix.id)
            }));
        }
    }

    const formatTimeDifference = (postedAt) => {
        const now = new Date();
        const tempPostedAt = new Date(postedAt);
        let offset = tempPostedAt.getTimezoneOffset() * -1;
        const postedDate = new Date(tempPostedAt.getTime() + offset * 60000);
        let differenceInSeconds = Math.floor(((now - postedDate) / 1000));
        if (differenceInSeconds < 60) {
            return `${differenceInSeconds} second${differenceInSeconds === 1 ? '' : 's'} ago`;
        } else if (differenceInSeconds < 3600) {
            const minutes = Math.floor(differenceInSeconds / 60);
            return `${minutes} minute${minutes === 1 ? '' : 's'} ago`;
        } else if (differenceInSeconds < 86400) {
            const hours = Math.floor(differenceInSeconds / 3600);
            return `${hours} hour${hours === 1 ? '' : 's'} ago`;
        }
        const days = Math.floor(differenceInSeconds / 86400);
        return `${days} day${days === 1 ? '' : 's'} ago`;

    };

    return (
        <div key={fix.id}
        className="relative bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]">
            <div className="flex flex-row items-center justify-between">
                <span>By: {fix.username}</span>
                <span>{formatTimeDifference(fix.postedAt)}</span>
            </div>
            <p className=" text-[2vh] bg-black text-lime-300 border border-lime-400 p-[1vh] break-words">{fix.content}</p>
            <div className="flex flex-row items-center gap-[1vh] mt-[1vh]">
                <div
                    onClick={async () => {
                        await handleUpvoting(fix, user, cookies, showErrorToast, setFixes, setUser);
                    }}
                    className={'text-center text-3xl hover:text-black transition block cursor-pointer ' + (checkIfUpvoted(fix.id, user) ? '' : 'text-gray-400')}>
                    <FontAwesomeIcon icon={faArrowUp}/>
                </div>
                <div
                    className="text-center text-3xl transition block">
                    {fix.votes}
                </div>
                <div
                    onClick={async () => {
                        await handleDownvoting(fix, user, cookies, showErrorToast, setFixes, setUser);
                    }}
                    className={"text-center text-3xl hover:text-black transition block cursor-pointer " + (checkIfDownvoted(fix.id, user) ? '' : 'text-gray-400')}>
                    <FontAwesomeIcon icon={faArrowDown}/>
                </div>
            </div>
        </div>
    );
}
