import React, { useEffect, useRef, useState } from "react";
import { Chat, ChevronRight, Feedback, HelpCenter, Logout, Notifications, Person, Search, Settings } from "@mui/icons-material";
import "./topbar.css";
import { Link, useNavigate } from "react-router-dom";
import authService from "../../services/auth.service";
import Notification from "../notification/Notification";

export default function Topbar({setCurrentUser}) {
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [notificationOpen, setNotificationOpen] = useState(false);
    const [closedByInteraction, setClosedByInteraction] = useState(true);
    const navigator = useNavigate();
    const handleDropdownToggle = () => {
        setDropdownOpen(!dropdownOpen);
        setNotificationOpen(false);
        setClosedByInteraction(true); 
    };

    const handleNotificationToggle = () => {
        setNotificationOpen(!notificationOpen);
        setDropdownOpen(false);
        setClosedByInteraction(true); 
    };
    console.log(dropdownOpen)
    const handleLogOut = () => {
        console.log("logged out")
        authService.logout();
        setCurrentUser(null)
        navigator("/login");
        localStorage.clear();
    }
    const optionsRef = useRef(null);
    useEffect(() => {
        function handleClickOutside(event) {
            if (optionsRef.current && !optionsRef.current.contains(event.target) && !closedByInteraction) {
                setDropdownOpen(false);
                setNotificationOpen(false);
            } else {
                setClosedByInteraction(false); // Reset flag when interaction occurs
            }
        }

        function handleScrollOutside(event) {
            if (optionsRef.current && !optionsRef.current.contains(event.target) && !closedByInteraction) {
                setDropdownOpen(false);
                setNotificationOpen(false);
            } else {
                setClosedByInteraction(false); // Reset flag when interaction occurs
            }
        }

        document.addEventListener("mousedown", handleClickOutside);
        document.addEventListener("scroll", handleScrollOutside);

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
            document.removeEventListener("scroll", handleScrollOutside);
        };
    }, [closedByInteraction]);
    return (
        <div className="topbarContainer">
            <div className="topbarLeft">
                <a className="logo" href="/">CMU</a>
            </div>
            <div className="topbarCenter">
                <div className="searchBar">
                    <Search className="searchIcon"/>
                    <input placeholder="Search for ..." className="searchInput" />
                </div>
            </div>
            <div className="topbarRight">
                <div className="topbarIcons">
                    <a className="topbarIconItem" onClick={handleNotificationToggle} >
                        <Notifications />
                        <span className="topbarIconBadge">
                            1
                        </span>
                    </a>
                    {notificationOpen && (
                            <div className="dropdownContent" ref={optionsRef}>
                                <Link to="/notifications" className="dropdownContentItemLink">
                                    <div className="dropdownContentItem">
                                        <span>See all Notifications</span> 
                                    </div>
                                </Link>
                                <div className="notificationsWrapper" style={{ fontWeight: "smaller" }}>
                                    <Notification />
                                    <Notification />
                                    <Notification />
                                    <Notification />
                                    <Notification />
                                    <Notification />
                                </div>
                            </div>
                        )}
                    <a onClick={handleDropdownToggle}>
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" className="topbarImg" />
                    </a>
                    {dropdownOpen && (
                            <div className="dropdownContent" ref={optionsRef}>
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
                                {/* <Link className="dropdownContentItemLink" > */}
                                <a className="dropdownContentItemLink">   
                                    <div className="dropdownContentItem" onClick={handleLogOut}>
                                        <Logout className="linkIcon" />
                                        <span>Log Out</span>
                                    </div>
                                </a>
                                    
                                {/* </Link> */}
                            </div>
                        )}
                </div>
            </div>
        </div>
    );
}
