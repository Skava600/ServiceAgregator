import { Link } from "react-router-dom";
import { Button, Stack, Tab, Tabs } from "@mui/material";
import "./navbar.less";
import { useAppSelector } from "../../state/store";
import { getIsSignedIn } from "../../state/selectors/userSelectors";

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
    const isSignedIn = useAppSelector(getIsSignedIn);
    return (
        <div className="app-navbar">
            <Tabs
                value={tabIndex}
                variant="scrollable"
                sx={{ width: "100%", padding: 2, boxSizing: "border-box" }}
            >
                <LinkTab label="Заказы" href="/tasks" />
                <LinkTab label="Исполнители" href="/profiles" />
                <div className="actions">
                    <Stack spacing={1} direction="row">
                        {tabIndex !== 0 && (
                            <Link to={isSignedIn ? "/create-task" : "/login"}>
                                <Button variant="outlined" size="small">
                                    Стать исполнителем
                                </Button>
                            </Link>
                        )}
                        {tabIndex !== 1 && (
                            <Link to={isSignedIn ? "/create-task" : "/login"}>
                                <Button variant="outlined" size="small">
                                    Разместить заказ
                                </Button>
                            </Link>
                        )}
                    </Stack>
                </div>
            </Tabs>
        </div>
    );
};
