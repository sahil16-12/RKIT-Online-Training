import { BankAccount } from "./Account.js";

/*
  Main app file
*/
const acc1 = new BankAccount("Sahil", 8000);
const acc2 = new BankAccount("Danish", 4000);

acc1.withdraw(2000);
acc2.deposit(1000);

console.log(acc1);
console.log(acc2);
console.log(BankAccount.info());
