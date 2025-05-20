import React from 'react';
import {useNavigate} from "react-router-dom";

export default function ProfilePageProblemComponent({problems}) {
    const navigate = useNavigate();

    return (
        <div id="problemHolder" className="flex flex-col gap-[1vh]">
            {problems.map(problem => {
                return (
                    <div className="text-[2vh] w-[85vw] bg-black text-lime-300 border border-lime-400 p-[1vh] break-words">
                        {problem.title}
                    </div>
                )
            })}
        </div>
    );
}