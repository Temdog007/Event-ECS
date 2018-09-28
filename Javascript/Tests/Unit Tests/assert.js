console.assertIs = function(actual, expected)
{
  console.assertTrue(actual instanceof expected, {actual : actual, expected : expected, errorMsg : "Actual isn't an instance of expected"});
}

console.assertError = function(func)
{
  var hasError = false;
  try
  {
    func();
    hasError = false;
  }
  catch(e)
  {
    hasError = true;
  }
  console.assertTrue(hasError, {msg : "Function did not give an error"});
}

console.assertIsNot = function(actual, expected)
{
  console.assertFalse(actual instanceof expected, {actual : actual, expected : expected, errorMsg : "Actual is an instance of expected"});
}

console.assertEquals = function(actual, expected, msg)
{
  console.assert(actual == expected, msg || {actual : actual, expected : expected, errorMsg : "These should be equal"});
}

console.assertNotEquals = function(actual, expected, msg)
{
  console.assert(actual != expected, msg || {actual : actual, expected : expected, errorMsg : "These should not be equal"});
}

console.assertTrue = function(actual, msg)
{
  console.assertEquals(actual, true, msg);
}

console.assertFalse = function(actual, msg)
{
  console.assertEquals(actual, false, msg);
}

console.assertNull = function(actual, msg)
{
  console.assertEquals(actual, null, msg);
}

console.assertNotNull = function(actual, msg)
{
  console.assertNotEquals(actual, null, msg);
}
