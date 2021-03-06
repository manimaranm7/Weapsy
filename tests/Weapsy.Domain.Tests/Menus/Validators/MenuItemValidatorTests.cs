﻿using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Menus.Validators;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Menus.Validators
{
    [TestFixture]
    public class MenuItemValidatorTests
    {
        [Test]
        [Ignore("Need to find extension to validate collection of validators")]
        public void Should_set_localisation_validator_when_validate_add_menu_item_command()
        {
            var updateMenuItem = new MenuItemDetails
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Link,
                PageId = Guid.NewGuid(),
                Link = "link",
                Text = "Text",
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>
                {
                    new MenuItemDetails.MenuItemLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Text = "Text 1",
                        Title = "Title 1"
                    },
                    new MenuItemDetails.MenuItemLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Text = "Text 2",
                        Title = "Title 2"
                    }
                }
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveChildValidator(x => x.MenuItemLocalisations, typeof(MenuItemLocalisationValidator));
        }

        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new MenuItemDetails
            {
                SiteId = Guid.Empty
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new MenuItemDetails
            {
                SiteId = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_if_page_id_is_empty()
        {
            var updateMenuItem = new MenuItemDetails
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Page,
                PageId = Guid.Empty,
                Link = "link",
                Text = "Text",
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>()
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.PageId, updateMenuItem);
        }

        [Test]
        public void Should_have_validation_error_if_page_does_not_exist()
        {
            var updateMenuItem = new MenuItemDetails
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Page,
                PageId = Guid.NewGuid(),
                Link = "link",
                Text = "Text",
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>()
            };

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.DoesPageExist(updateMenuItem.SiteId, updateMenuItem.PageId)).Returns(false);

            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.PageId, updateMenuItem);
        }

        [Test]
        public void Should_have_validation_error_if_link_is_empty()
        {
            var updateMenuItem = new MenuItemDetails
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Link,
                PageId = Guid.Empty,
                Link = string.Empty,
                Text = "Text",
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>()
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Link, updateMenuItem);
        }

        [Test]
        public void Should_have_validation_error_if_link_is_too_long()
        {
            var link = string.Empty;
            for (int i = 0; i < 251; i++) link += i.ToString();

            var updateMenuItem = new MenuItemDetails
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Link,
                PageId = Guid.Empty,
                Link = link,
                Text = "Text",
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>()
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Link, updateMenuItem);
        }

        [Test]
        public void Should_have_validation_error_if_text_is_too_long()
        {
            var text = string.Empty;
            for (int i = 0; i < 101; i++) text += i.ToString();

            var updateMenuItem = new MenuItemDetails
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Link,
                PageId = Guid.Empty,
                Link = "Link",
                Text = text,
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>()
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Text, updateMenuItem);
        }

        [Test]
        public void Should_have_validation_error_if_title_is_too_long()
        {
            var title = string.Empty;
            for (int i = 0; i < 101; i++) title += i.ToString();

            var updateMenuItem = new MenuItemDetails
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Link,
                PageId = Guid.Empty,
                Link = "Link",
                Text = "Text",
                Title = title,
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>()
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Title, updateMenuItem);
        }

        [Test]
        public void Should_have_error_when_localisations_are_missing()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.AreAllSupportedLanguagesIncluded(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>())).Returns(false);
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new MenuItemValidator<MenuItemDetails>(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.MenuItemLocalisations, new MenuItemDetails());
        }
    }
}
