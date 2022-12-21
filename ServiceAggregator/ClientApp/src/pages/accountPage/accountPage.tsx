import { useEffect, useState } from "react";
import {
    Typography,
    Paper,
    Divider,
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Grid,
    Card,
} from "@mui/material";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { ProfileInfo, Page, TaskCard } from "../../components";
import { useAppSelector } from "../../state/store";
import { getToken, getUser } from "../../state/selectors/userSelectors";
import { getTasks } from "../../api";
import { ITask } from "../../api/interfaces";
import "./accountPage.less";

const INITIAL_STATE = {
    expanded: "panel1",
    myTasks: [] as ITask[],
    myOrders: [] as ITask[],
};

export const AccountPage = () => {
    const [expanded, setExpanded] = useState<string | false>(
        INITIAL_STATE.expanded
    );
    const [myTasks, setMyTasks] = useState(INITIAL_STATE.myTasks);
    const [myOrders, setMyOrders] = useState(INITIAL_STATE.myOrders);
    const user = useAppSelector(getUser);
    const token = useAppSelector(getToken);

    useEffect(() => {
        getTasks({ slugs: [], isMyOrders: true, token }).then(({ data }) =>
            setMyTasks(data)
        );
    }, [token]);

    const handleChange = (panel: string) => (_: any, isExpanded: boolean) => {
        setExpanded(isExpanded ? panel : false);
    };

    return (
        <Page className="account-page">
            <Grid container spacing={4}>
                <Grid item xs={12} md={5}>
                    <Paper className="account-card" elevation={8}>
                        <AccountCircleIcon className="account-img" />
                        <Divider />
                        <p className="info-row name">
                            Яблонский Кирилл Дмитриевич
                        </p>
                        <p className="info-row">blackshark564@gmail.com</p>
                        <p className="info-row">+375447010025</p>
                    </Paper>
                </Grid>
                <Grid item xs={12} md={12} lg={7}>
                    <Accordion
                        expanded={expanded === "panel1"}
                        onChange={handleChange("panel1")}
                    >
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel1bh-content"
                            id="panel1bh-header"
                        >
                            <Typography sx={{ width: "33%", flexShrink: 0 }}>
                                Ваши заказы
                            </Typography>
                            <Typography sx={{ color: "text.secondary" }}>
                                Заказы, которые вы разместили ранее
                            </Typography>
                        </AccordionSummary>
                        <Divider />
                        <AccordionDetails>
                            {myTasks.map((task) => (
                                <TaskCard task={task} isMine />
                            ))}
                            {!myTasks.length && (
                                <Card className="no-data">
                                    <Typography>
                                        Вы не разместили ни одного заказа...
                                    </Typography>
                                </Card>
                            )}
                        </AccordionDetails>
                    </Accordion>
                    <Accordion
                        expanded={expanded === "panel2"}
                        onChange={handleChange("panel2")}
                    >
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel2bh-content"
                            id="panel2bh-header"
                        >
                            <Typography sx={{ width: "33%", flexShrink: 0 }}>
                                Запросы работ
                            </Typography>
                            <Typography sx={{ color: "text.secondary" }}>
                                Отклики на ваши заявки
                            </Typography>
                        </AccordionSummary>
                        <Divider />
                        <AccordionDetails>
                            <Typography>
                                Donec placerat, lectus sed mattis semper, neque
                                lectus feugiat lectus, varius pulvinar diam eros
                                in elit. Pellentesque convallis laoreet laoreet.
                            </Typography>
                        </AccordionDetails>
                    </Accordion>
                </Grid>
                <Grid item xs={12}>
                    <ProfileInfo user={user!} />
                </Grid>
            </Grid>
        </Page>
    );
};
