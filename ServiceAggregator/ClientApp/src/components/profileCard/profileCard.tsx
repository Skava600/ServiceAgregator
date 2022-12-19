import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Card, CardContent, Rating } from "@mui/material";
import FaceIcon from "@mui/icons-material/Face";
import Face2Icon from "@mui/icons-material/Face2";
import Face3Icon from "@mui/icons-material/Face3";
import Face4Icon from "@mui/icons-material/Face4";
import Face5Icon from "@mui/icons-material/Face5";
import Face6Icon from "@mui/icons-material/Face6";
import WorkIcon from "@mui/icons-material/Work";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import { IProfile } from "../../api/interfaces";
import "./profileCard.less";
import classNames from "classnames";

const reviewWordForms = ["отзыв", "отзыва", "отзывов"];
const orderWordForms = ["заказ", "заказа", "заказов"];
const sectionWordForms = ["раздел", "раздела", "разделов"];

function getWordForm(n: number, wordForms: string[]) {
    n = Math.abs(n) % 100;
    var n1 = n % 10;

    if (n > 10 && n < 20) return wordForms[2];

    if (n1 > 1 && n1 < 5) return wordForms[1];

    if (n1 === 1) return wordForms[0];

    return wordForms[2];
}

const styleSummaryItem = (n: number, wordForms: string[]) => {
    return `${n || 0} ${getWordForm(n, wordForms)}`;
};

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
    profile: IProfile;
    variant?: TVariant;
    message?: string;
    isChosen?: boolean;
};

export const ProfileCard = ({
    profile,
    variant = "full",
    message,
    isChosen,
}: TProps) => {
    const [avatar, setAvatar] = useState<any>();

    useEffect(() => {
        setAvatar(getRandomAvatar());
    }, []);

    return (
        <Link to={`/profile/${profile.id}`} className="profile-link">
            <Card className={classNames("profile-card", variant)}>
                {isChosen && <CheckCircleOutlineIcon color="success" />}
                <div className="profile-card-row">
                    {avatar}
                    <div className="profile-card-content">
                        <p className="profile-name">{profile.doerName}</p>
                        {variant === "full" && (
                            <p className="profile-description">
                                {profile.doerDescription}
                            </p>
                        )}
                        {variant === "short" && (
                            <p className="profile-message">{message}</p>
                        )}
                    </div>
                </div>
                <div className="profile-summary">
                    <Rating value={profile.rating} size="small" readOnly />
                    <span className="ratings-text">{`${styleSummaryItem(
                        profile.reviewsCount,
                        reviewWordForms
                    )}, выполнил ${styleSummaryItem(
                        profile.orderCount,
                        orderWordForms
                    )}`}</span>
                    {variant === "full" && (
                        <div className="work-sections">
                            <WorkIcon fontSize="small" />
                            <span className="work-sections-text">
                                {profile.sections[0].name}
                                {profile.sections.length > 1 &&
                                    `
                                    +
                                    ${styleSummaryItem(
                                        profile.sections.length,
                                        sectionWordForms
                                    )}
                                `}
                            </span>
                        </div>
                    )}
                </div>
            </Card>
        </Link>
    );
};
