import { Category, EmojiObjects, Event, Group, HelpOutline, Notifications, RssFeed, School, WorkOutline } from "@mui/icons-material";
import "../sidebar.css";
import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import apis from "../../../services/apis.service";

export default function StudentSidebar() {
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
    console.log(facultyOptions)
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
                            {facultyOptions.map((faculty, index) => (
                                <Link key={index} to={`/feed/${faculty.id}`} className="sidebarListItemLink">
                                    <li className="sidebarListItem">{faculty.name}</li>
                                </Link>
                            ))}
                        </ul>
                    )}
                    <Link to="/submission" className="sidebarListItemLink"> 
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
