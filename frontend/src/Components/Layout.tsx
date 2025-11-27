import React, { ReactNode } from 'react';
import Navbar from './Navbar';
import { Outlet } from 'react-router-dom';
import "./Layout.css"

interface Props {
    children: ReactNode;
}

const Layout = () => {
    return (
    <div className='pageContainer'>
        <Navbar />
        <div className='content'>
        <Outlet /> {/* This renders the page content */}
        </div>
    </div>
    );
};

export default Layout;
