import React from 'react';
import {useLocation, useNavigate} from "react-router-dom";

export default function NavButton({ to, label , setOpen }) {
    const navigate = useNavigate();
    const location = useLocation();

    return (
        <button
            onClick={() => {
                navigate(to);
                setOpen(false);
            }}
            className={`text-[2.2vh] tracking-wider hover:text-yellow-300 ${
                location.pathname === to ? "underline" : ""
            }`}
        >
            {label}
        </button>
    );
}