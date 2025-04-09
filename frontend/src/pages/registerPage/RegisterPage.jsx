import React from 'react';
import car from '../../assets/CAR.png'
import lift from '../../assets/LIFT.png'
import {useEffect, useState} from 'react'
import {Link, useNavigate} from "react-router-dom";
import {useCookies} from "react-cookie";
import {toast} from 'react-hot-toast';

export default function RegisterPage({setUserLoginCookies}) {
    const [carLifted, setCarLifted] = useState(false);
    const [newUserData, setNewUserData] = useState({});
    const [submittable, setSubmittable] = useState(false);
    const navigate = useNavigate();
    const showSuccessToast = (data) => toast.success(data.message);
    const showErrorToast = (data) => toast.error(data.message);
    const [cookies] = useCookies(['user']);

    useEffect(() => {
        if (cookies.user) {
            navigate('/');
        }
    }, [cookies]);

    useEffect(() => {
        if (
            newUserData.username &&
            newUserData.email &&
            newUserData.password
        ) {
            setSubmittable(true);
        }
    }, [newUserData]);

    useEffect(() => {
        const timer = setTimeout(() => {
            setCarLifted(true);
        }, 2500);

        return () => clearTimeout(timer);
    }, []);

    const handleSignupRequest = async () => {
        const response = await fetch('/api/Users/signup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newUserData),
        });
        return await response.json();
    }

    const handleSignup = (event) => {
        event.preventDefault();
        if (newUserData.password.match(/([a-z?'!0-9])/gi).join('') ===
        newUserData.password) {
            handleSignupRequest(event).then((data) => {
                if (data.message) {
                    showErrorToast(data.message);
                } else {
                    showSuccessToast({message: "Successfully registered"});
                    setUserLoginCookies(data.sessionToken);
                    navigate('/');
                }
            });
        } else {
            showErrorToast({message: 'Invalid Credentials'});
        }
    }

    return (
        <div className="relative flex justify-center items-start min-h-screen bg-gradient-to-br from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono p-10 overflow-hidden">

            <img
                src={car}
                alt="car"
                className={`absolute w-120 transition-all duration-2000 ease-in-out ${carLifted ? 'top-24' : 'top-[50%]'} z-10`}
                style={{ left: '34.4%' }}
            />

            <img
                src={lift}
                alt="lift"
                className="absolute w-120 bottom-0 z-0"
                style={{ left: '34.4%' }}
            />

            <div className={'absolute w-120 top-[30%] h-auto flex justify-center items-center'}
            style={{ left: '34.4%'}}>
            <form
                className={`relative w-80 top-[30%] z-20 bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-5 rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)] transition-opacity duration-4000 ${
                    carLifted ? 'opacity-100' : 'opacity-0 pointer-events-none'
                }`}
            >
                <h5 className="text-2xl font-bold underline mb-4 text-center drop-shadow-[2px_2px_0_rgba(0,0,0,0.6)]">
                    Registration
                </h5>
                <div className="flex flex-col gap-3">
                    <label className="text-sm tracking-wider">Username:</label>
                    <input
                        onChange={(event) => {
                            setNewUserData({...newUserData, username: event.target.value});
                        }}
                        type="text"
                        required
                        placeholder="Enter your username"
                        className="bg-black text-lime-300 border border-lime-400 p-1"
                    />
                    <label className="text-sm tracking-wider">Email:</label>
                    <input
                        onChange={(event) => {
                            setNewUserData({...newUserData, email: event.target.value})
                        }}
                        type="email"
                        required
                        placeholder="Enter your email"
                        className="bg-black text-lime-300 border border-lime-400 p-1"
                    />
                    <label className="text-sm tracking-wider">Password:</label>
                    <input
                        onChange={(event) => {
                            setNewUserData({...newUserData, password: event.target.value})
                        }}
                        type="password"
                        required
                        placeholder="********"
                        className="bg-black text-lime-300 border border-lime-400 p-1"
                    />
                </div>
                <button
                    onClick={handleSignup}
                    disabled={!submittable}
                    className="w-full mt-4 bg-yellow-400 text-black font-extrabold py-2 rounded hover:bg-yellow-200 shadow-md border border-black"
                >
                    Register
                </button>
                <div className='mt-2'>
                      <span className='text-sm tracking-wider'>
                        {' '}
                          Already have an account?{' '}
                      </span>
                    <Link to='/login' className='font-medium text-blue-700'>
                        Login!
                    </Link>
                </div>
            </form>
            </div>
        </div>
    );
}

