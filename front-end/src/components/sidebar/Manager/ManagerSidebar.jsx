import { BarChart, Category, EmojiObjects, Notifications } from "@mui/icons-material"
import "../sidebar.css"
import { useState } from "react";
import { Link } from "react-router-dom";

export default function ManagerSidebar() {
    const [showArticlesDropdown, setShowArticlesDropdown] = useState(false);
    const [showSubmissionsDropdown, setShowSubmissionsDropdown] = useState(false);
    return (
        <div className="sidebar">
            <div className="sidebarWrapper">
                <ul className="sidebarList">
                    <Link to="/dashboard" className="sidebarListItemLink"> 
                        <li className="sidebarListItem" >
                            <BarChart className="sidebarIcon"/>
                            <span className="sidebarListItemText">Dashboard</span>
                            
                        </li>
                    </Link>
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
                    <li className="sidebarListItem" onClick={() => setShowSubmissionsDropdown(!showSubmissionsDropdown)}>
                        <Category className="sidebarIcon"/>
                        <span className="sidebarListItemText">Submissions</span>
                        {showSubmissionsDropdown ? <span className="dropdownIcon">▲</span> : <span className="dropdownIcon">▼</span>}
                    </li>
                    {showSubmissionsDropdown && (
                        <ul className="sidebarDropdownContent">
                            <Link to="/" className="sidebarListItemLink"> 
                                <li className="sidebarListItem">Facility 1</li>
                            </Link>
                            <Link to="/" className="sidebarListItemLink"> 
                                <li className="sidebarListItem">Facility 2</li>
                            </Link>
                        </ul>
                    )}
                    <li className="sidebarListItem" >
                        <Notifications className="sidebarIcon"/>
                        <span className="sidebarListItemText">Notifications</span>
                        
                    </li>
                </ul>
                {/* <button className="sidebarButton">
                    Show More
                </button> */}
                <hr className="sidebarHr"/>
                <ul className="sidebarFriendList">
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>
                    <li className="sidebarFriend">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="" className="sidebarFriendImg" />
                        <span className="sidebarFriendName">Jane Doe </span>
                    </li>

                </ul>
            </div>
        </div>
    )
}
