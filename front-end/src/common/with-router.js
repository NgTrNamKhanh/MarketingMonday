import { useEffect } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import authService from "../services/auth.service";

export const withRouter = (Component) => {
    function ComponentWithRouterProp(props) {
        let location = useLocation();
        let navigate = useNavigate();
        let params = useParams();
        return <Component {...props} router={{ location, navigate, params }} />;
    }
    return ComponentWithRouterProp;
};
export const ProtectedRoute = ({ element, requiredRoles }) => {
    const user = authService.getCurrentUser();
    const navigate = useNavigate();
    console.log(user)
    useEffect(() => {
        // Check if the user's role matches the required role
        console.log(user)
        if (user != null && (Array.isArray(user.roles) && requiredRoles.some(role => user.roles.includes(role)))) {
            // Allow rendering the protected component
        } else {
            // Redirect to another route if the role doesn't match
            navigate('/unauthorized');
        }
    }, [navigate, user, requiredRoles]);

    return element;
};