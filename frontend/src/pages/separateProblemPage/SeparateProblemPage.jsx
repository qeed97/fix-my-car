import React, {useEffect, useState} from 'react';
import {useCookies} from "react-cookie";
import {useParams} from "react-router-dom";
import {toast} from "react-hot-toast";
import SeparateProblemComponent from "../../components/separateProblemComponent/SeparateProblemComponent.jsx";
import FixComponent from "../../components/fixComponent/FixComponent.jsx";
import FixFormComponent from "../../components/fixFormComponent/FixFormComponent.jsx";

export default function SeparateProblemPage({setUserLoginCookies}) {
    const [cookies] = useCookies(['user']);
    const {problemId} = useParams();
    const [problemData, setProblemData] = useState(null);
    const [fixes, setFixes] = useState([]);
    const [reply, setReply] = useState({content: ""});
    const [submittable, setSubmittable] = useState(false);
    //const [user, setUser] = useState(null);
    const showErrorToast = (message) => toast.error(message);
    console.log(problemId);
    const fetchFixes = async (problemId, setFixes) => {
        try {
            const res = await fetch('/api/Fix?problemId=' + problemId);
            const data = await res.json();
            if (data.message){
                showErrorToast(data.message);
                return;
            }
            setFixes(data);
            console.log(data);
        } catch (error) {
            console.log(error);
        }
    }
    useEffect(() => {
        const fetchProblem = async () => {
            try {
                const res = await fetch('/api/Problems/' + problemId, {
                    headers: {
                        Authorization: "Bearer " + cookies.user
                    }
                });
                const data = await res.json();
                if (data.message){
                    showErrorToast(data.message);
                    return;
                }
                setProblemData(data);
            } catch (error) {
                console.log(error);
            }
        }
        //fetchUser();
        //checkIfAdmin();
        fetchFixes(problemId, setFixes);
        fetchProblem();
    },[]);

    useEffect(() => {
        if (reply.content.length > 0) {
            setSubmittable(true)
            return;
        }
        setSubmittable(false);
    }, [reply])

    console.log(problemData);
    return problemData && (
        <div className="relative flex flex-col items-center justify-center gap-4 bg-gradient-to-br from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono p-[8vh]">
            <SeparateProblemComponent problem={problemData} setProblem={setProblemData}/>
            <div className="flex flex-col w-[90vw] gap-[1vh] pb-[1vh]">
                {fixes.map(fix => {
                    return (
                        <FixComponent key={fix.id} fix={fix} problem={problemData} setFixes={setFixes} setProblemData={setProblemData}/>
                    )
                    }
                )}
            </div>
            <FixFormComponent problemId={problemId} reply={reply} submittable={submittable} cookies={cookies}
                              setReply={setReply} setFixes={setFixes} setSubmittable={setSubmittable} fetchFixes={fetchFixes} />
        </div>
    );
}