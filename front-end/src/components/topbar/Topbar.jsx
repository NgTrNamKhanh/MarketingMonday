import React, { useState } from "react";
import { Chat, ChevronRight, Feedback, HelpCenter, Logout, Notifications, Person, Search, Settings } from "@mui/icons-material";
import "./topbar.css";
import { Link } from "react-router-dom";

export default function Topbar() {
    const [dropdownOpen, setDropdownOpen] = useState(false);

    const handleDropdownToggle = () => {
        setDropdownOpen(!dropdownOpen);
    };

    return (
        <div className="topbarContainer">
            <div className="topbarLeft">
                <a className="logo" href="/">Social</a>
            </div>
            <div className="topbarCenter">
                <div className="searchBar">
                    <Search className="searchIcon"/>
                    <input placeholder="Search for ..." className="searchInput" />
                </div>
            </div>
            <div className="topbarRight">
                <div className="topbarLinks">
                    <span className="topbarLink">
                        HomePage
                    </span>
                    <span className="topbarLink">
                        TimeLine
                    </span>
                </div>
                <div className="topbarIcons">
                    <div className="topbarIconItem">
                        <Person  />
                        <span className="topbarIconBadge">
                            1
                        </span>
                        
                    </div>
                    <div className="topbarIconItem">
                        <Chat/>
                        <span className="topbarIconBadge">
                            2
                        </span>
                    </div>
                    <Link to="/notifications" className="topbarIconItem">
                        <Notifications />
                        <span className="topbarIconBadge">
                            1
                        </span>
                    </Link>
                    <a onClick={handleDropdownToggle}>
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" className="topbarImg" />
                    </a>
                    {dropdownOpen && (
                            <div className="dropdownContent">
                                <Link to="/profile" className="dropdownContentItemLink">
                                    <div className="dropdownContentItem">
                                            <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" className="topbarImg linkIcon" />
                                            <span>Nigga</span>
                                            <ChevronRight className="moreIcon"/>
                                    </div>
                                </Link>
                                <hr className="dropdownHr"/>
                                <Link to="/settings" className="dropdownContentItemLink">
                                    <div className="dropdownContentItem">
                                            <Settings className="linkIcon" />
                                            <span>Settings</span> 
                                            <ChevronRight className="moreIcon"/>
                                    </div>
                                </Link>
                                <Link to="/settings" className="dropdownContentItemLink">
                                    <div className="dropdownContentItem">
                                            <HelpCenter className="linkIcon" />
                                            <span>Help and Support</span> 
                                            <ChevronRight className="moreIcon"/>
                                    </div>
                                </Link>
                                <Link to="/feedback" className="dropdownContentItemLink">
                                    <div className="dropdownContentItem">
                                            <Feedback className="linkIcon" />
                                            <span>Feedback</span> 
                                    </div>
                                </Link>
                                <Link to="/login" className="dropdownContentItemLink">
                                    <div className="dropdownContentItem">
                                        <Logout className="linkIcon" />
                                        <span>Log Out</span>
                                    </div>
                                </Link>
                            </div>
                        )}
                </div>
            </div>
        </div>
    );
}
