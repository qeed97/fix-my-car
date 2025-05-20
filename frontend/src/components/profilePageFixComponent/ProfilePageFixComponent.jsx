import React from 'react';
import {useNavigate} from "react-router-dom";

export default function ProfilePageFixComponent({fixes}) {
    const navigate = useNavigate();
    console.log(fixes);
    return (
        <div id="fixHolder" className="flex flex-col gap-[1vh]">
            {fixes.map(fix => {
                return (
                    <div>
                        <div>
                            problem title
                        </div>
                        <div className="text-[2vh] w-[85vw] bg-black text-lime-300 border border-lime-400 p-[1vh] break-words">
                            {fix.content}
                        </div>
                    </div>
                )
            })}
        </div>
    );
}