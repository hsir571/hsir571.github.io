const result = document.getElementById("result"); 

function check() {
  const input = document.getElementById('text-input').value;
  const filteredInput = input.replace(/[^a-zA-Z0-9]/g,'').toLowerCase();
  
  if (filteredInput.trim() === "") {
    alert("Please input a value")
    result.innerText = "";
    return;
  }

  const reverse = filteredInput.split('').reverse().join('').toLowerCase();
  
  if(reverse === filteredInput){ 
    result.innerText = `${input} is a palindrome`;
  }else{
    result.innerText = `${input} is not a palindrome`;
  }


}
