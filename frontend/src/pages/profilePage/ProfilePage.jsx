import React, {useEffect, useState} from 'react';
import {useNavigate} from "react-router-dom";
import {useCookies} from "react-cookie";
import ProfilePageProblemComponent from "../../components/profilePageProblemComponent/ProfilePageProblemComponent.jsx";
import ProfilePageFixComponent from "../../components/profilePageFixComponent/ProfilePageFixComponent.jsx";

export default function ProfilePage({setUserLoginCookies}) {
    const [user, setUser] = useState(null);
    const [selectedTab, setSelectedTab] = useState(null);
    const navigate = useNavigate();
    const [cookies] = useCookies(['user']);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const res = await fetch('/api/Users/GetBySessionToken',
                    {
                        headers: {
                            'Authorization': "Bearer " + cookies.user
                        }
                    });
                const data = await res.json();
                setUser(data);
            } catch (e) {
                console.error(e);
            }
        };
        fetchUser();
    }, []);

    useEffect(() => {
        if (!cookies.user){
            navigate("/login");
        }
    }, [cookies]);


    return user && (
        <div className="relative flex flex-col items-center min-h-screen bg-gradient-to-br from-blue-800 via-purple-700 to-pink-600 text-yellow-100 font-mono p-[8vh] ">
            <div className="relative w-[90vw] h-[17vh] bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]">
                <div>
                    <div>
                        <h1 className="h-[7vh] text-[3vh] w-full bg-black text-lime-300 border border-lime-400 p-[1vh] truncate">{user.username}</h1>
                    </div>
                    <div className="py-[1vh]">
                        <div>Karma:
                            <span className="p-[1vh] text-lime-300">{user.karma}</span>
                        </div>
                    </div>
                </div>
            </div>
            <div className="py-[3vh] flex flex-row justify-center items-center gap-[2vh]">
                <span className={
                    "text-lime-300" +
                    (selectedTab === 'problem' ? " underline" : " hover:underline")}
                      onClick={() => setSelectedTab('problem')}
                >Problems</span>
                <span className={"text-lime-300" + (selectedTab === 'fix' ? " underline" : " hover:underline")} onClick={() => setSelectedTab('fix')}>Fixes</span>
            </div>
            <div className="flex flex-col items-center gap-[2vh]">
                {selectedTab && (selectedTab === 'problem' ? (
                    <ProfilePageProblemComponent problems={user.problems}/>
                ) : (
                    <ProfilePageFixComponent fixes={user.fixes} />
                ))}
            </div>
        </div>
    );
}