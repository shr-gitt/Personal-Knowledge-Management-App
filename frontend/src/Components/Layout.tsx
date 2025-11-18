import React, { ReactNode } from 'react';
import Navbar from './Navbar';

interface Props {
    children: ReactNode;
}

const Layout = ({ children }: Props) => {
    return (
    <div>
        <Navbar />
        <div style={{ padding: '20px' }}>
        {children} {/* This renders the page content */}
        </div>
    </div>
    );
};

export default Layout;
