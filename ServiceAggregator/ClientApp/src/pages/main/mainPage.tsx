import { useState } from "react";
import { Link } from "react-router-dom";
import { Tab, Tabs, Typography } from "@mui/material";
import { Page, AppNavbar } from "../../components/";
import "./mainPage.less";

type LinkTabProps = {
    label: string;
    href: string;
};

function LinkTab({ label, href }: LinkTabProps) {
    return (
        <Link className="tab-link" to={href}>
            <Tab label={label} />
        </Link>
    );
}

export const MainPage = () => {
    return (
        <Page className="main-page" navbarTabIndex={-1}>
            <div className="content">
                <Typography variant="h1" color="white">
                    Добро пожаловать!
                </Typography>
            </div>
        </Page>
    );
};

export default MainPage;
