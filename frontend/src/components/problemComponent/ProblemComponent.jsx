import React from 'react';

export default function ProblemComponent({ problems }) {
    return problems && (
        <div>
            {problems.map((problem) => {
                return (<div key={problem.id}>
                        <h2>{problem.title}</h2>
                        <p>{problem.description}</p>
                </div>)
            })}
        </div>
    );
};