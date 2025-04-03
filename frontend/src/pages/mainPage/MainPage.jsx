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
                setNormalProblem(() => [...normalProblem, ...data.problems]);
                setsearchProblem(() => [...searchProblem, ...data.problems]);
                setStartIndex(() => data.index);
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
            <div>
                <ProblemComponent problems={searchProblem}/>
            </div>
        </> ):
        <div>
            Lading
        </div>

};