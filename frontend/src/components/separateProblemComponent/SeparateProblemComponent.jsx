import React from 'react';
import {useCookies} from "react-cookie";

export default function SeparateProblemComponent({problem, setProblem, user}) {
    const [cookies] = useCookies(['user']);

    return (
        <div className="relative w-[90vw] bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]">
            <h1 className="text-4xl underline font-bold mb-[2vh] text-center drop-shadow-[2px_2px_0_rgba(0,0,0,0.6)]">
                {problem.title}
            </h1>
            <p className="text-xl tracking-wider">
                {problem.description}
            </p>
        </div>
    );
}

