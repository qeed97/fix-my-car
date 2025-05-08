import React from 'react';
import {useNavigate} from "react-router-dom";

export default function ProblemComponent({ problems }) {
    const navigate = useNavigate();

    return problems && (
        <div className="flex flex-col gap-[1vh] pb-[1vh]">
            {problems.map((problem) => {
                return (<div className="relative w-screen h-[17vh] bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]" key={problem.id}>
                        <h2 className="h-[7vh] text-[3vh] w-full bg-black text-lime-300 border border-lime-400 p-[1vh] truncate" onClick={() => {
                            navigate(`/problem/${problem.id}`);
                        }}>{problem.title}</h2>
                        <p className="h-[5vh] text-[2vh] bg-black text-lime-300 border border-lime-400 p-[1vh] truncate">{problem.description}</p>
                </div>)
            })}
        </div>
    );
};