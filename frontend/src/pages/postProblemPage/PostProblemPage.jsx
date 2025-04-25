import React, {useEffect, useState} from 'react';
import {useCookies} from "react-cookie";
import {useNavigate} from "react-router-dom";
import {toast} from "react-hot-toast";

export default function PostProblemPage() {
    const [cookies] = useCookies(['user']);
    const navigate = useNavigate();
    const [problem, setProblem] = useState({});
    const [submittable, setSubmittable] = useState(false);
    const showErrorToast = (message) => toast.error(message);
    const showSuccessToast = (message) => toast.success(message);

    useEffect(() => {
        if (problem.title &&
            problem.description) {
            setSubmittable(true);
        }
    }, [problem]);

    useEffect(() => {
        if (!cookies.user) {
            navigate('/login');
        }
    }, [cookies]);

    const createProblem = async () => {
        const res = await fetch('/api/Problems', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                'Authorization': "Bearer " + cookies.user
            },
            body: JSON.stringify(problem),
        });
        return await res.json();
    }

    const handleSubmit = (event) => {
        event.preventDefault();
        createProblem(event).then((data) => {
            if (data.message) {
                showErrorToast(data.message);
                return;
            }
            showSuccessToast("Successfully created problem");
            navigate('/');
        });
    }

    return (
        <div className="relative flex justify-center items-start min-h-screen bg-gradient-to-br from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono p-[8vh]">
            <div className="flex flex-col justify-center items-center">
                <h1 className="text-4xl underline font-bold mb-[2vh] text-center drop-shadow-[2px_2px_0_rgba(0,0,0,0.6)]">
                    Post a Problem
                </h1>
                <form className="relative w-[95vw] h-[76vh] bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]">
                    <div className="flex flex-col gap-[1vh] pb-[1vh]">
                        <label className="text-xl tracking-wider">
                            Title
                        </label>
                        <textarea
                            onChange={(event) => {
                                setProblem({...problem, title: event.target.value});
                            }}
                            required
                            placeholder="Enter title"
                            className="bg-black text-lime-300 border border-lime-400 p-[1vh] h-[8vh]"
                        >
                        </textarea>
                    </div>
                    <div className="flex flex-col gap-[1vh] pb-[1vh]">
                        <label className="text-xl tracking-wider">
                            Description
                        </label>
                        <textarea
                        onChange={(event) => {
                            setProblem({...problem, description: event.target.value});
                        }}
                        required
                        placeholder="Enter description"
                        className="bg-black text-lime-300 border border-lime-400 p-[1vh] h-[42vh]"
                        >
                        </textarea>
                    </div>
                    <button
                    disabled={!submittable}
                    onClick={handleSubmit}
                    className="w-full mt-[2vh] bg-yellow-400 text-black font-extrabold py-[1vh] rounded hover:bg-yellow-200 shadow-md border border-black"
                    >
                        Submit
                    </button>
                </form>
            </div>
        </div>
    );
}