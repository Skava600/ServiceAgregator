import { Link } from "react-router-dom";
import { Button, Stack, Tab, Tabs } from "@mui/material";
import "./navbar.less";

type LinkTabProps = {
    label: string;
    href: string;
};

const LinkTab = ({ label, href }: LinkTabProps) => {
    return (
        <Link className="tab-link" to={href}>
            <Tab label={label} />
        </Link>
    );
};

type TProps = {
    tabIndex: number;
};

export const AppNavbar = ({ tabIndex }: TProps) => {
    return (
        <div className="app-navbar">
            <Tabs
                value={tabIndex}
                variant="scrollable"
                sx={{ width: "100%", padding: 2, "box-sizing": "border-box" }}
            >
                <LinkTab label="Заказы" href="/tasks" />
                <LinkTab label="Исполнители" href="/profiles" />
                <div className="actions">
                    <Stack spacing={1} direction="row">
                        <Button variant="outlined" size="small">
                            Стать исполнителем
                        </Button>
                        <Button variant="contained" size="small">
                            Разместить заказ
                        </Button>
                    </Stack>
                </div>
            </Tabs>
        </div>
    );
};
