import { useCallback, useEffect, useState } from "react";
import {
    Card,
    Checkbox,
    FormControlLabel,
    Grid,
    Paper,
    Typography,
} from "@mui/material";
import TreeView from "@mui/lab/TreeView";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import TreeItem from "@mui/lab/TreeItem";
import { Page, ProfileCard } from "../../components/";
import { getProfiles, getWorkSections } from "../../api";
import { IWorkCategory, IWorkSection, IProfile } from "../../api/interfaces";
import FaceIcon from "@mui/icons-material/Face";
import "./profilesPage.less";

const INITIAL_STATE = {
    workSections: [] as IWorkCategory[],
    selectedSections: [] as IWorkSection[],
    profiles: [] as IProfile[],
};

export const ProfilesPage = () => {
    const [workSections, setWorkSections] = useState(
        INITIAL_STATE.workSections
    );
    const [profiles, setProfiles] = useState(INITIAL_STATE.profiles);
    const [selectedSections, setSelectedSections] = useState(
        INITIAL_STATE.selectedSections
    );

    const fetchProfiles = useCallback(() => {
        getProfiles(selectedSections.map(({ slug }) => slug)).then(({ data }) =>
            setProfiles(data)
        );
    }, [selectedSections]);

    useEffect(() => {
        getWorkSections().then(({ data }) => setWorkSections(data));
        fetchProfiles();
    }, [fetchProfiles]);

    const handleAddSection = (section: IWorkSection, shouldAdd: boolean) => {
        const index = selectedSections.findIndex(
            (s) => s.slug === section.slug
        );
        const exists = index !== -1;

        if (shouldAdd && !exists) {
            setSelectedSections((v) => [...v, section]);
            return;
        }

        if (!shouldAdd && exists) {
            const sections = [...selectedSections];
            sections.splice(index, 1);
            console.log(sections.map(({ slug }) => slug).join());
            setSelectedSections(sections);
        }
    };

    return (
        <Page className="profile-page" navbarTabIndex={1}>
            <div className="divider">
                <div className="work-sections-tree">
                    <TreeView
                        defaultCollapseIcon={<ExpandMoreIcon />}
                        defaultExpandIcon={<ChevronRightIcon />}
                    >
                        {workSections.map((category) => (
                            <TreeItem
                                nodeId={category.name}
                                label={category.name}
                            >
                                {category.sections.map(({ name, slug }) => {
                                    const isSelected = selectedSections.some(
                                        (sect) => sect.slug === slug
                                    );
                                    return (
                                        <TreeItem
                                            nodeId={slug}
                                            label={
                                                <FormControlLabel
                                                    label={name}
                                                    control={
                                                        <Checkbox
                                                            checked={isSelected}
                                                            onChange={() =>
                                                                handleAddSection(
                                                                    {
                                                                        name,
                                                                        slug,
                                                                    },
                                                                    !isSelected
                                                                )
                                                            }
                                                        />
                                                    }
                                                />
                                            }
                                        />
                                    );
                                })}
                            </TreeItem>
                        ))}
                    </TreeView>
                </div>
                <div className="right-side">
                    {profiles.map((profile) => (
                        <ProfileCard profile={profile} />
                    ))}
                    {!profiles.length && (
                        <Card className="no-profiles">
                            <b>По вашему запросу пока что нет исполнителей</b>
                        </Card>
                    )}
                </div>
            </div>
        </Page>
    );
};
