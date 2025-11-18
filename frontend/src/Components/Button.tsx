import React, { ReactNode } from 'react';

interface Props {
    children: ReactNode;
    onClick: () => void;
    color: string;
}

const Button = ({ children, onClick, color }: Props) => {
    return (
    <button
        className={"btn btn-" + color}
        onClick={onClick}
        style={{
        padding: '10px 20px',
        backgroundColor: color === 'primary' ? 'blue' : 'gray',
        color: 'white',
        border: 'none',
        borderRadius: '5px',
        }}
    >
    {children}
    </button>
    );
};

export default Button;
