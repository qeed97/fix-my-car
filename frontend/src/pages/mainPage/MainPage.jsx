import React, {useEffect, useState} from 'react';
import {useCookies} from "react-cookie";
import ProblemComponent from "../../components/problemComponent/ProblemComponent.jsx";

export default function MainPage({ searchProblem, normalProblem, setsearchProblem, setNormalProblem, /*setUserLoginCookies*/ }) {
    const [startIndex, setStartIndex] = useState(0);
    //const [cookies] = useCookies(['user']);
    const [allProblems, setAllProblems] = useState(false);

    const fetchProblems = async () => {
        try {
            if (!allProblems) {
                const res = await fetch(`/api/Problems?startIndex=${startIndex}`, {
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json",
                    }
                });
                const data = await res.json();
                console.log(data);
                if (data.problems.length === 0){
                    setAllProblems(() => true);
                    return;
                }
                setStartIndex(() => data.index);
                if (startIndex <= 10){
                    setNormalProblem(data.problems);
                    setsearchProblem(data.problems);
                } else {
                    setNormalProblem(() => [...normalProblem, ...data.problems]);
                    setsearchProblem(() => [...searchProblem, ...data.problems]);
                }
            }
        } catch (error){
            console.log(error)
        }
    };

    useEffect(() => {
        setNormalProblem(() => []);
        setsearchProblem(() => []);
        fetchProblems();
    }, []);

    return searchProblem ? (
        <>
            <div className="relative flex justify-center items-start min-h-screen bg-gradient-to-br from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono p-[8vh] ">
                <ProblemComponent problems={searchProblem}/>
            </div>
        </> ):
        <div>
            Lading
        </div>

};