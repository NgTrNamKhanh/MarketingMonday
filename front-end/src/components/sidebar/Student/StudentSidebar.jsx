import { Category, EmojiObjects, Event, Group, HelpOutline, Notifications, RssFeed, School, WorkOutline } from "@mui/icons-material";
import "../sidebar.css";
import { Link } from "react-router-dom";
import { useState } from "react";

export default function StudentSidebar() {
    const [showArticlesDropdown, setShowArticlesDropdown] = useState(false);

    return (
        <div className="sidebar">
            <div className="sidebarWrapper">
                <ul className="sidebarList">
                    <li className="sidebarListItem" onClick={() => setShowArticlesDropdown(!showArticlesDropdown)}>
                        <EmojiObjects className="sidebarIcon"/>
                        <span className="sidebarListItemText">Articles</span>
                        {showArticlesDropdown ? <span className="dropdownIcon">▲</span> : <span className="dropdownIcon">▼</span>}
                            
                    </li>
                    {showArticlesDropdown && (
                        <ul className="sidebarDropdownContent">
                            <Link to="/" className="sidebarListItemLink"> 
                                <li className="sidebarListItem">Facility 1</li>
                            </Link>
                            <Link to="/" className="sidebarListItemLink"> 
                                <li className="sidebarListItem">Facility 2</li>
                            </Link>
                        </ul>
                    )}
                    <Link to="/student/submission" className="sidebarListItemLink"> 
                        <li className="sidebarListItem">
                            <Category className="sidebarIcon"/>
                            <span className="sidebarListItemText">Add a Submission</span>
                        </li>
                    </Link>
                </ul>
                <hr className="sidebarHr"/>
                <ul className="sidebarFriendList">
                </ul>
            </div>
        </div>
    );
}
