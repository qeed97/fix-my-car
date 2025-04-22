import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useCookies } from "react-cookie";
import { FaBars, FaTimes } from "react-icons/fa";
import logo from "../../assets/LOGO.png";
import NavBtn from "../navButton/NavButton.jsx"

export default function NavbarMobile({ setUserLoginCookies }) {
    const navigate = useNavigate();
    const [cookies] = useCookies(["user"]);
    const [cookiesCleared, setCookiesCleared] = useState(false);
    const [open, setOpen] = useState(false);

    const handleLogout = async () => {
        await fetch("/api/Users/logout", {
            method: "POST",
            headers: { Authorization: "Bearer " + cookies.user },
        });
        setUserLoginCookies(null);
        setCookiesCleared(true);
    };

    useEffect(() => {
        if (cookiesCleared) {
            navigate("/login");
            setCookiesCleared(false);
            setOpen(false);
        }
    }, [cookiesCleared, navigate]);

    return (
        <header className="fixed top-0 left-0 w-[100vw] h-[8vh] flex items-center justify-between px-[3vw] bg-gradient-to-r from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono shadow-lg z-50">
            <div
                className="flex items-center gap-[1vw] cursor-pointer"
                onClick={() => navigate("/")}
            >
                <img src={logo} alt="logo" className="h-[30vh] w-[30vw]" />
            </div>

            <button
                aria-label="toggle menu"
                onClick={() => setOpen(!open)}
                className="text-[4vh] focus:outline-none"
            >
                {open ? <FaTimes /> : <FaBars />}
            </button>

            {open && (
                <div
                    className="absolute top-[8vh] right-0 w-[70vw] bg-[#000000cc] backdrop-blur-md border-l-4 border-yellow-300
                     flex flex-col gap-[3vh] py-[4vh] px-[4vw] shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]"
                >
                    <NavBtn to="/" label="Home" setOpen={setOpen} />
                    <NavBtn to="/postproblem" label="Post a Problem" setOpen={setOpen} />
                    {cookies.user && <NavBtn to="/profile" label="Profile" setOpen={setOpen} />}
                    {cookies.user ? (
                        <button
                            onClick={handleLogout}
                            className="text-[2.2vh] tracking-wider hover:text-yellow-300"
                        >
                            Logout
                        </button>
                    ) : (
                        <>
                            <NavBtn to="/login" label="Login" setOpen={setOpen} />
                            <NavBtn to="/register" label="Register" setOpen={setOpen}/>
                        </>
                    )}
                </div>
            )}
        </header>
    );
}