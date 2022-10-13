using System.Windows.Documents;
using System.Windows.Media;
using VerselineSelector.WPF.Services;

namespace VerselineSelector.Tests.Services;

public class TextHighlightingServiceTests
{
    [Test, Timeout(2000)]
    [TestCase("")]
    public void HighlightWithTokenize_NoSearchToken_ShouldReturnOneInlineWithoutBackgroundBrush(string searchText)
    {
        // Arrange
        var highlighter = new TextHighlightingService();
        var textToParse = "De specialiteit komt slechts voor vergoeding in aanmerking als ze is toegediend";        

        // Act
        var inlines = highlighter.Highlight(textToParse, searchText, null, true);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(inlines, Has.Count.EqualTo(1));
            Assert.That(((Run)inlines[0]).Text, Is.EqualTo(textToParse));            
            Assert.That(inlines[0].Background, Is.Null);
            
        });
    }

    [Test]
    [TestCase("specialiteit")]
    public void HighlightWithTokenize_OneSearchToken_ShouldReturnThreeInlinesOfWhichOneWithBackgroundBrushYellow(string searchText)
    {
        // Arrange
        var highlighter = new TextHighlightingService();
        var textToParse = "De specialiteit komt slechts voor vergoeding in aanmerking als ze is toegediend";        

        // Act
        var inlines = highlighter.Highlight(textToParse, searchText, null, true);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(inlines, Has.Count.EqualTo(3));

            Assert.That(((Run)inlines[0]).Text, Is.EqualTo("De "));
            Assert.That(((Run)inlines[1]).Text, Is.EqualTo(searchText));
            Assert.That(((Run)inlines[2]).Text, Is.EqualTo(" komt slechts voor vergoeding in aanmerking als ze is toegediend"));

            Assert.That(inlines[0].Background, Is.Null);
            Assert.That(inlines[1].Background, Is.EqualTo(Brushes.Yellow));
            Assert.That(inlines[2].Background, Is.Null);
        });
    }

    [Test]
    [TestCase("specialiteit aanmerking")]
    [TestCase("aanmerking specialiteit")]
    public void HighlightWithTokenize_TwoSearchTokens_ShouldReturnFiveInlinesOfWhichTwoWithBackgroundBrushYellow(string searchText)
    {
        // Arrange
        var highlighter = new TextHighlightingService();
        var textToParse = "De specialiteit komt slechts voor vergoeding in aanmerking als ze is toegediend";        

        // Act
        var inlines = highlighter.Highlight(textToParse, searchText, null, true);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(inlines, Has.Count.EqualTo(5));

            Assert.That(((Run)inlines[0]).Text, Is.EqualTo("De "));
            Assert.That(((Run)inlines[1]).Text, Is.EqualTo("specialiteit"));
            Assert.That(((Run)inlines[2]).Text, Is.EqualTo(" komt slechts voor vergoeding in "));
            Assert.That(((Run)inlines[3]).Text, Is.EqualTo("aanmerking"));
            Assert.That(((Run)inlines[4]).Text, Is.EqualTo(" als ze is toegediend"));

            Assert.That(inlines[0].Background, Is.Null);
            Assert.That(inlines[1].Background, Is.EqualTo(Brushes.Yellow));            
            Assert.That(inlines[2].Background, Is.Null);
            Assert.That(inlines[3].Background, Is.EqualTo(Brushes.Yellow));
            Assert.That(inlines[4].Background, Is.Null);
        });
    } 
}
