import { BarChart, Category, EmojiObjects, Notifications } from "@mui/icons-material"
import "../sidebar.css"
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import apis from "../../../services/apis.service";

export default function ManagerSidebar() {
    const [showSubmissionsDropdown, setShowSubmissionsDropdown] = useState(false);
    const [showArticlesDropdown, setShowArticlesDropdown] = useState(false);
    const [facultyOptions, setFacultyOptions] = useState([]);
    const fetchFaculties = async () => {
        try {

            const facultiesResponse = await axios.get(
                apis.faculty
            );
            localStorage.setItem("faculties", JSON.stringify(facultiesResponse.data),
            setFacultyOptions(facultiesResponse.data)
            );
        } catch (error) {
            console.error("Error fetching faculties:", error);
            // setMessage("Error fetching roles and faculties");
        }
    } ;
    useEffect(() => {
        const initializeData = async () => {
            const faLocal = localStorage.getItem("faculties");
            try {
                if (
                    faLocal &&
                    JSON.parse(faLocal).length !== 0
                ) {
                    const facultiesFromStorage = JSON.parse(faLocal);
                    setFacultyOptions(facultiesFromStorage);
                } else {
                    await fetchFaculties();
                }
            } catch (error) {
                console.error("Error initializing data:", error);
                // setMessage("Error initializing data");
            }
        
        }
        initializeData();
    }, []);
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
                            {facultyOptions.map((faculty, index) => (
                                <Link key={index} to={`/feed/${faculty.id}`} className="sidebarListItemLink">
                                    <li className="sidebarListItem">{faculty.name}</li>
                                </Link>
                            ))}
                        </ul>
                    )}
                    <li className="sidebarListItem" onClick={() => setShowSubmissionsDropdown(!showSubmissionsDropdown)}>
                        <Category className="sidebarIcon"/>
                        <span className="sidebarListItemText">Submissions</span>
                        {showSubmissionsDropdown ? <span className="dropdownIcon">▲</span> : <span className="dropdownIcon">▼</span>}
                    </li>
                    {showSubmissionsDropdown && (
                        <ul className="sidebarDropdownContent">
                            {facultyOptions.map((faculty, index) => (
                                <Link key={index} to={`/submissions/${faculty.id}`} className="sidebarListItemLink">
                                    <li className="sidebarListItem">{faculty.name}</li>
                                </Link>
                            ))}
                        </ul>
                    )}
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
