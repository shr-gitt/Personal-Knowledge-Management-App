import React, { ReactNode } from 'react';
import Navbar from './Navbar';
import { Outlet } from 'react-router-dom';

interface Props {
    children: ReactNode;
}

const Layout = () => {
    return (
    <div style={{display:"flex"}}>
        <Navbar />
        <div style={{marginLeft:"160px", padding: '20px', width:"100%" }}>
        <Outlet /> {/* This renders the page content */}
        </div>
    </div>
    );
};

export default Layout;
