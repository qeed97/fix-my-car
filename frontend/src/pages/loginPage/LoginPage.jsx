import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useCookies } from "react-cookie";
import { toast } from "react-hot-toast";
import car from "../../assets/CAR.png";
import lift from "../../assets/LIFT.png";

export default function LoginPage({ setUserLoginCookies }) {
    const [carLifted, setCarLifted] = useState(false);
    const navigate = useNavigate();
    const [userDetails, setUserDetails] = React.useState({
        email: null,
        password: null,
    });
    const [cookies] = useCookies(['user']);
    const showErrorToast = (message) => toast.error(message);
    const showSuccessToast = (message) => toast.success(message);

    useEffect(() => {
        if (cookies.user) {
            navigate('/');
        }
    }, [cookies]);

    useEffect(() => {
        const timer = setTimeout(() => {
            setCarLifted(true);
        }, 2500);

        return () => clearTimeout(timer);
    }, []);

    const handleLoginRequest = async () => {
        const response = await fetch('/api/Users/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userDetails),
        });
        return await response.json();
    }

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const data = await handleLoginRequest();
            if (data.message) return showErrorToast(data.message);
            setUserLoginCookies(data);
            showSuccessToast("Successfully logged in!");
        } catch {
            showErrorToast("Invalid credentials");
        }
    };

    const onChange = (field) => (e) =>
        setUserDetails({ ...userDetails, [field]: e.target.value });

    return (
        <div className="relative flex justify-center items-start min-h-screen bg-gradient-to-br from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono p-[3vh] overflow-hidden">
            <img
                src={car}
                alt="car"
                className={`absolute left-1/2 -translate-x-1/2 w-[55vw] max-w-[260px] transition-all duration-[2000ms] ease-in-out
                ${carLifted ? "top-[47vh]" : "top-[65vh]"} z-10`}
            />

            <img
                src={lift}
                alt="lift"
                className="absolute left-1/2 -translate-x-1/2 w-[55vw] max-w-[260px] bottom-0 z-0"
            />

            {/* ----------- Form wrapper ----------- */}
            <div
                className="absolute left-1/2 -translate-x-1/2 flex justify-center z-20 items-center w-[55vw] max-w-[260px]"
                style={{ top: "38vh" }}
            >
                <form
                    className={`relative w-[100%] z-20 bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[3vh] rounded-md
                      shadow-[0_0_20px_5px_rgba(255,255,0,0.8)] transition-opacity duration-[1800ms]
                      ${carLifted ? "opacity-100" : "opacity-0 pointer-events-none"}`}
                >
                    <h5 className="text-[3vh] font-bold underline mb-[2vh] text-center drop-shadow-[2px_2px_0_rgba(0,0,0,0.6)]">
                        Login
                    </h5>

                    <div className="flex flex-col gap-[1.5vh]">
                        <label className="text-[1.6vh] tracking-wider">Email:</label>
                        <input
                            onChange={onChange("email")}
                            type="email"
                            required
                            placeholder="Enter your email"
                            className="bg-black text-lime-300 border border-lime-400 p-[1vh]"
                        />

                        <label className="text-[1.6vh] tracking-wider">Password:</label>
                        <input
                            onChange={onChange("password")}
                            type="password"
                            required
                            placeholder="********"
                            className="bg-black text-lime-300 border border-lime-400 p-[1vh]"
                        />
                    </div>

                    <button
                        onClick={handleLogin}
                        className="w-full mt-[3vh] bg-yellow-400 text-black font-extrabold py-[1.6vh] rounded hover:bg-yellow-200 shadow-md border border-black"
                    >
                        Login
                    </button>

                    <div className="mt-[1.5vh] text-center">
                        <span className="text-[1.6vh] tracking-wider">Not registered? </span>
                        <Link to="/register" className="font-medium text-blue-300 underline">
                            Sign up!
                        </Link>
                    </div>
                </form>
            </div>
        </div>
    );
}