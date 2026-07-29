# Bilingual README and Promotion Article Design

## Goal

Prepare GitHub-ready bilingual project documentation for Folder Theme Studio v0.1.0-beta.6: a complete repository README and a standalone Markdown announcement suitable for GitHub and open-source communities.

## Deliverables

- Replace `README.md` with a polished Chinese-first, English-second bilingual document.
- Create `docs/PROMOTION.zh-en.md` with a reusable bilingual promotional article.
- Keep existing user guides, release notes, security policy, contribution guide, license, and build instructions linked from the README.

## README structure

1. Logo, bilingual project name, version/platform/license badges, and one-sentence positioning.
2. Chinese section: project purpose, highlights, Beta 6 changes, installation, quick start, operating modes, important notes, documentation, development, and license.
3. English section with equivalent information and links.
4. Screenshot placeholders only when they are explicit and easy for maintainers to replace; do not invent screenshots or download URLs.

## Promotion article structure

1. A concise bilingual headline and opening problem statement.
2. What the application changes and what it deliberately does not replace.
3. Visual customization, image import, safe plan/apply/restore workflow, recursive monitoring, tray operation, and Windows sign-in startup.
4. Beta 6 highlights, intended users, installation steps, project status, and contribution invitation.
5. Chinese full article followed by a complete English version.

## Editorial rules

- Professional, restrained, and suitable for an open-source release.
- Do not claim universal Windows compatibility or completed code signing.
- State Windows 10/11 x64, unsigned Beta status, Apache-2.0, and the online .NET 8 Desktop Runtime prerequisite accurately.
- Avoid unsupported performance, security, popularity, and compatibility claims.
- Use concise Markdown, scannable headings, and stable relative repository links.

## Acceptance criteria

- Both files render as valid Markdown and contain complete Chinese and English content.
- README references v0.1.0-beta.6 consistently and preserves reproducible build instructions.
- Promotional article is reusable outside the repository after replacing only the download/repository link.
- No placeholder facts, fabricated URLs, or contradictions with Beta 6 release notes.
