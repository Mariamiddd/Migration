namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //arrange
            int a = 5;
            int b = 10;

            //act 
            int c = a + b;

            //assert
            Assert.Equal(15, c);

        }
    }
}
