export const setToken = (token) => {
  localStorage.setItem('token', token);
};

export const getToken = () => {
  let token = localStorage.getItem('token');
  if (token === null || token === undefined) {
    return '';
  }
  return token;
};
