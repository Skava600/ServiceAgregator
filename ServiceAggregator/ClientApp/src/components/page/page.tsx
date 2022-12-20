import classNames from "classnames";
import { Paper } from "@mui/material";
import { Header, AppNavbar } from "../";
import "./page.less";

type TProps = {
    children: React.ReactNode;
    className?: string;
    navbarTabIndex?: number;
};

export const Page = ({ children, className, navbarTabIndex }: TProps) => {
    return (
        <div className="page">
            <Paper className="page-paper">
                <Header />
                {navbarTabIndex !== undefined && (
                    <AppNavbar tabIndex={navbarTabIndex} />
                )}
                <div className={className}>{children}</div>
            </Paper>
        </div>
    );
};
