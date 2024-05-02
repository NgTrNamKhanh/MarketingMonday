// const endpoint = 'https://cmuniversity.somee.com/api/';
const endpoint = 'https://localhost:7218/api/';
const apis = {
    account: endpoint + "account/",
    user: endpoint + "user/",
    manager: endpoint + "manager/",
    admin: endpoint + "admin/",
    coordinator: endpoint + "coordinator/",
    faculty: endpoint + "Faculties/",
    role: endpoint + "Roles/",
    event: endpoint + "events/",
    article: endpoint + "Articles/",
    normal: endpoint,
    comment: endpoint + "comment/",
    like: endpoint + "like/",
    dislike: endpoint + "dislike/",
    notification: endpoint + "notification/",
};

export default apis;
