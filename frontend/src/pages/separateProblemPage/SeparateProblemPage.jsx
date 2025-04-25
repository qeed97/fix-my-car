import React, {useEffect, useState} from 'react';
import {useCookies} from "react-cookie";
import {useParams} from "react-router-dom";
import {toast} from "react-hot-toast";
import SeparateProblemComponent from "../../components/separateProblemComponent/SeparateProblemComponent.jsx";

export default function SeparateProblemPage({setUserLoginCookies}) {
    const [cookies] = useCookies(['user']);
    const {problemId} = useParams();
    const [problemData, setProblemData] = useState(null);
    //const [fixes, setFixes] = useState([]);
    //const [user, setUser] = useState(null);
    const showErrorToast = (message) => toast.error(message);
    console.log(problemId);
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
                setProblemData(() => data);
            } catch (error) {
                console.log(error);
            }
        }
        //fetchUser();
        //checkIfAdmin();

        fetchProblem();
    },[]);

    console.log(problemData);
    return problemData && (
        <div className="relative flex justify-center items-start min-h-screen bg-gradient-to-br from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono p-[8vh]">
            <SeparateProblemComponent problem={problemData} setProblem={setProblemData}/>
        </div>
    );
}