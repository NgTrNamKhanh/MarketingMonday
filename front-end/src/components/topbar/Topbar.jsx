import { Chat, Notifications, Person, Search } from "@mui/icons-material"
import "./topbar.css"
import { Link } from "react-router-dom"

export default function Topbar() {
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
                        <Person/>
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
                        <Notifications/>
                        <span className="topbarIconBadge">
                            1
                        </span>
                    </Link>
                    <a href="/profile">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" className="topbarImg" />
                    </a>
                    
                </div>
            </div>
        </div>
    )
}
