import React from 'react';
import {useCookies} from "react-cookie";

export default function SeparateProblemComponent({problem, setProblem, user}) {
    const [cookies] = useCookies(['user']);

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
        <div className="relative w-[90vw] bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]">
            <div className="flex flex-row items-center justify-between">
                <span>By: {problem.username}</span>
                <span>{formatTimeDifference(problem.postedAt)}</span>
            </div>
            <h1 className="text-4xl underline font-bold mb-[2vh] text-center drop-shadow-[2px_2px_0_rgba(0,0,0,0.6)]">
                {problem.title}
            </h1>
            <p className="text-xl tracking-wider">
                {problem.description}
            </p>
        </div>
    );
}

