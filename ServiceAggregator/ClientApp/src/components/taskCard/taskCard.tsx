import { useEffect, useState } from "react";
import moment from "moment";
import { Link } from "react-router-dom";
import { Button, Card, IconButton, Rating } from "@mui/material";
import FaceIcon from "@mui/icons-material/Face";
import Face2Icon from "@mui/icons-material/Face2";
import Face3Icon from "@mui/icons-material/Face3";
import Face4Icon from "@mui/icons-material/Face4";
import Face5Icon from "@mui/icons-material/Face5";
import Face6Icon from "@mui/icons-material/Face6";
import WorkIcon from "@mui/icons-material/Work";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import EditIcon from "@mui/icons-material/Edit";
import ScheduleIcon from "@mui/icons-material/Schedule";
import { ITask } from "../../api/interfaces";
import "./taskCard.less";

const AVATARS = [
    () => <FaceIcon className="avatar" />,
    () => <Face2Icon className="avatar" />,
    () => <Face3Icon className="avatar" />,
    () => <Face4Icon className="avatar" />,
    () => <Face5Icon className="avatar" />,
    () => <Face6Icon className="avatar" />,
];

const getRandomAvatar = () => AVATARS[Math.floor(Math.random() * 6)];

type TVariant = "short" | "full";

type TProps = {
    task: ITask;
    variant?: TVariant;
    isMine?: boolean;
    respondButton?: React.ReactNode;
};

const STATUSES = {
    Open: "Активный",
    Done: "Завершён",
    Canceled: "Отменён",
    InProgress: "Подтверждён",
};

export const TaskCard = ({
    task,
    variant = "full",
    isMine = false,
    respondButton,
}: TProps) => {
    const [avatar, setAvatar] = useState<any>();

    useEffect(() => {
        setAvatar(getRandomAvatar());
    }, []);

    return (
        <Link to={`/task/${task.id}`} className="task-link">
            <Card className="task-card">
                {isMine && (
                    <Link to={`/edit-task/${task.id}`}>
                        <IconButton>
                            <EditIcon />
                        </IconButton>
                    </Link>
                )}
                <div className="task-card-row">
                    {variant === "full" && avatar}
                    <div className="task-card-content">
                        <span className="name">{task.header}</span>
                        <span className="description">{task.text}</span>
                    </div>
                    <div className="task-card-price-container">
                        <span className="price">
                            {task.price === null
                                ? "Договорная"
                                : `${task.price}р.`}
                        </span>
                        <span className="status">{STATUSES[task.status]}</span>
                    </div>
                </div>

                <div className="task-summary">
                    <div>
                        <WorkIcon fontSize="small" />
                        <span className="info">{task.section.name}</span>
                    </div>
                    <div>
                        <LocationOnIcon fontSize="small" />
                        <span className="info">{task.location}</span>
                    </div>
                    <div>
                        <ScheduleIcon fontSize="small" />
                        <span className="info">
                            {moment(task.expireDate)
                                .locale("ru")
                                .format("[До] Do MMMM YYYY")}
                        </span>
                    </div>
                </div>
                {respondButton}
            </Card>
        </Link>
    );
};
