import { EmojiObjects, Notifications } from "@mui/icons-material"
import "../sidebar.css"

export default function ManagerSidebar() {
    return (
        <div className="sidebar">
            <div className="sidebarWrapper">
                <ul className="sidebarList">
                    <li className="sidebarListItem">
                        <EmojiObjects className="sidebarIcon"/>
                        <span className="sidebarListItemText">Articles</span>
                    </li>
                    <li className="sidebarListItem">
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
