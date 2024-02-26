import "./profile.css"

export default function Profile() {
    return (
        <div className="profile">
            <div className="profileRight">
                <div className="profileRightTop">
                    <div className="profileCover">
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" className="profileCoverImg" />
                        <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" className="profileUserImg" />
                    </div>
                </div>
                <div className="profileInfo">
                    <h4 className="profileInfoName">NIgga</h4>
                    <span className="profileInfoDesc">NIgga</span>
                </div>
                <div className="profileRightBottom"></div>
            </div>
        
        </div>
    )
}
