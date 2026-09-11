namespace DsaPractice.Complexity.Tests;

public class Exercise04_ComplexityQuizTests
{
    [Fact]
    public void SnippetA_ArrayIndex() => Assert.Equal(BigO.Constant, ComplexityQuiz.SnippetA());

    [Fact]
    public void SnippetB_HalvingLoop() => Assert.Equal(BigO.Logarithmic, ComplexityQuiz.SnippetB());

    [Fact]
    public void SnippetC_SingleLoop() => Assert.Equal(BigO.Linear, ComplexityQuiz.SnippetC());

    [Fact]
    public void SnippetD_NestedLoops() => Assert.Equal(BigO.Quadratic, ComplexityQuiz.SnippetD());

    [Fact]
    public void SnippetE_LoopWithDoublingInnerLoop() => Assert.Equal(BigO.Linearithmic, ComplexityQuiz.SnippetE());

    [Fact]
    public void SnippetF_NaiveFibonacci() => Assert.Equal(BigO.Exponential, ComplexityQuiz.SnippetF());

    [Fact]
    public void SnippetG_SequentialLoops() => Assert.Equal(BigO.Linear, ComplexityQuiz.SnippetG());

    [Fact]
    public void SnippetH_ConstantInnerLoop() => Assert.Equal(BigO.Linear, ComplexityQuiz.SnippetH());

    [Fact]
    public void SnippetI_SquareRootLoop() => Assert.Equal(BigO.SquareRoot, ComplexityQuiz.SnippetI());
}
