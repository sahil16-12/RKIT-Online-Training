const users = [
    { name: "John", role: "admin" },
    { name: "Jane", role: "user" },
    { name: "Alex", role: "admin" }
  ];

// Filter admins using $.grep()
  const admins = $.grep(users, (user) => {
            return user.role ==="admin";
  })
// Extract admin names using $.map()
  const namesofAdmins = $.map(users, (user) => {
    return user.name;
  })
// Merge objects using $.extend()
  const defaultSettings  = {theme: "dark", notifications: true}

  const userPreference = {theme: "light"};

  const extendedObject = $.extend({},defaultSettings, userPreference)

  console.log("Extended object"  , extendedObject);