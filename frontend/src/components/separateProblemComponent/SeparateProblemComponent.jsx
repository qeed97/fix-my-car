import React from 'react';
import {useCookies} from "react-cookie";

export default function SeparateProblemComponent({problem, setProblem}) {
    const [cookies] = useCookies(['user']);

    return (
        <div>
            <h1 className="text-4xl underline font-bold mb-[2vh] text-center drop-shadow-[2px_2px_0_rgba(0,0,0,0.6)]">
                {problem.title}
            </h1>
            <p className="text-xl tracking-wider">
                {problem.description}
            </p>
            <div className="flex flex-col gap-[1vh] pb-[1vh]">
                <h2 className="text-xl tracking-wider">
                    Fixes
                </h2>
            </div>
        </div>
    );
}

