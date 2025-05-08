import React from 'react';
import {toast} from "react-hot-toast";
import {useNavigate} from "react-router-dom";

export default function FixComponent({fix,problem,setFixes,setProblemData}) {
    const showErrorToast = (message) => toast.error(message);
    const navigate = useNavigate();
    //const deleteFix = async () => {}

    return (
        <div key={fix.id}
        className="relative bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]">
            <p className=" text-[2vh] bg-black text-lime-300 border border-lime-400 p-[1vh] break-words">{fix.content}</p>
        </div>
    );
}
