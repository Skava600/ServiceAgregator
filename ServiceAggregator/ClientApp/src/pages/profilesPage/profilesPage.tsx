import { useEffect } from "react";
import { Page } from "../../components/";
import TreeView from "@mui/lab/TreeView";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import TreeItem from "@mui/lab/TreeItem";
import "./profilesPage.less";
import { getWorkSections } from "../../api";

export const ProfilesPage = () => {
    useEffect(() => {
        getWorkSections().then(console.log);
    }, []);
    return (
        <Page navbarTabIndex={1}>
            <TreeView
                defaultCollapseIcon={<ExpandMoreIcon />}
                defaultExpandIcon={<ChevronRightIcon />}
            >
                <TreeItem nodeId="1" label="Applications">
                    <TreeItem nodeId="2" label="Calendar" />
                </TreeItem>
            </TreeView>
        </Page>
    );
};
