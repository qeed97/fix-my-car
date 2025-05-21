import React from 'react';
import {toast} from "react-hot-toast";

export default function FixFormComponent({problemId, reply, submittable, cookies, setReply, setFixes, setSubmittable, fetchFixes}) {
    const showErrorToast = (message) => toast.error(message);
    const showSuccessToast = (message) => toast.success(message);

    const postFix = async (problemId, cookies, reply, showErrorToast) => {
        try {

            const res = await fetch('/api/Fix?problemId=' + problemId, {
                method: "POST",
                headers: {
                    'Authorization': "Bearer " + cookies.user,
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({content: reply.content, postedAt: new Date(Date.now()).toISOString()}),
            });
            return await res.json();
        } catch (error) {
            showErrorToast("something went wrong");
            console.error(error);
        }
    }



    return (
            <div className="flex flex-col justify-center items-center">
                <form className="relative w-[95vw] h-[30vh] bg-[#000000aa] backdrop-blur-md border-4 border-yellow-300 p-[2vh] rounded-md shadow-[0_0_20px_5px_rgba(255,255,0,0.8)]">
                    <div className="flex flex-col gap-[1vh] pb-[1vh]">
                        <label className="text-xl tracking-wider">
                            Your Reply
                        </label>
                        <textarea
                            onChange={(event) => {
                                setReply({...reply, content: event.target.value});
                            }}
                            id="reply"
                            rows="5"
                            placeholder="Write your reply here..."
                            className="bg-black text-lime-300 border border-lime-400 p-[1vh] h-[10vh]"
                        >
                        </textarea>
                    </div>
                    <button
                        onClick={async (event) => {
                            event.preventDefault();
                            const data = await postFix(problemId, cookies, reply, showErrorToast);
                            if (data.message) {
                                showErrorToast(data.message);
                                return;
                            }
                            showSuccessToast("Successfully posted answer!");
                            await fetchFixes(problemId, setFixes);
                            setReply({content: ""});
                            setSubmittable(false);
                        }}
                        disabled={!submittable}
                        type="submit"
                        className="w-full mt-[2vh] bg-yellow-400 text-black font-extrabold py-[1vh] rounded hover:bg-yellow-200 shadow-md border border-black"
                    >
                        Submit
                    </button>
                </form>
            </div>
    );
}

