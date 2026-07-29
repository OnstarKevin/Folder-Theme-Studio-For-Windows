# Bilingual README and Promotion Article Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Deliver a polished bilingual GitHub README and a standalone bilingual Markdown announcement for Folder Theme Studio v0.1.0-beta.6.

**Architecture:** `README.md` remains the repository entry point, with a complete Chinese section followed by an equivalent English section. `docs/PROMOTION.zh-en.md` is an independent publication-ready article that reuses verified project facts but does not depend on repository-only context for readability.

**Tech Stack:** GitHub-flavored Markdown, repository-relative links, UTF-8 text.

## Global Constraints

- Product version is exactly `v0.1.0-beta.6` for Windows 10/11 x64.
- License is Apache-2.0; published binaries are unsigned Beta builds.
- The installer may download the x64 .NET 8 Desktop Runtime when it is missing.
- Do not invent screenshots, download URLs, benchmarks, compatibility claims, or security guarantees.
- Preserve links to the user guides, release notes, test checklist, security policy, contribution guide, and license.

---

### Task 1: Rewrite the bilingual repository README

**Files:**
- Modify: `README.md`

**Interfaces:**
- Consumes: verified Beta 6 behavior from `docs/releases/v0.1.0-beta.6.md` and usage details from both user guides.
- Produces: the GitHub repository landing page linked by the release package.

- [ ] **Step 1: Write the Chinese README section**

Include the logo, concise positioning, badges, feature highlights, Beta 6 changes, installation, a five-step quick start, operating-mode comparison, notes, documentation links, build commands, contribution guidance, and license.

- [ ] **Step 2: Write the equivalent English section**

Mirror the Chinese information without machine-like sentence-by-sentence duplication. Keep commands and relative links identical where appropriate.

- [ ] **Step 3: Validate factual and link consistency**

Run:

```powershell
rg -n "v0\.1\.0-beta\.6|Windows 10/11 x64|Apache-2\.0|USER-GUIDE|SECURITY|CONTRIBUTING" README.md
rg -n "beta\.5|beta5|https?://example" README.md
git diff --check -- README.md
```

Expected: all required facts and links are present; the second search returns no matches; `git diff --check` exits successfully.

- [ ] **Step 4: Commit the README**

```powershell
git add README.md
git commit -m "docs: rewrite bilingual project readme"
```

### Task 2: Create the bilingual open-source announcement

**Files:**
- Create: `docs/PROMOTION.zh-en.md`

**Interfaces:**
- Consumes: claims already verified in the rewritten README and Beta 6 release notes.
- Produces: a self-contained Markdown article for GitHub Discussions and open-source communities.

- [ ] **Step 1: Write the complete Chinese article**

Use a publication-ready title and cover: the ordinary-folder customization problem, project boundaries, visual palette, image import, plan/apply/restore workflow, recursive monitoring, tray and sign-in startup, intended users, installation, Beta status, and contribution invitation.

- [ ] **Step 2: Write the complete English article**

Provide a natural English counterpart with the same scope and accuracy. Link to `[项目仓库 / Project repository](../README.md)`; publishers can replace this relative link with the public repository URL when reposting elsewhere.

- [ ] **Step 3: Validate Markdown and claims**

Run:

```powershell
rg -n "v0\.1\.0-beta\.6|Windows 10/11 x64|Apache-2\.0|Project repository" docs/PROMOTION.zh-en.md
rg -n "beta\.5|beta5|example\.com" docs/PROMOTION.zh-en.md
git diff --check -- docs/PROMOTION.zh-en.md
```

Expected: required facts and the single publishing field are present; no stale-version or vague placeholder matches are present; whitespace validation succeeds.

- [ ] **Step 4: Review both deliverables together**

Run:

```powershell
git diff -- README.md docs/PROMOTION.zh-en.md
```

Expected: Chinese and English sections agree on features, prerequisites, version, platform, signing status, and license.

- [ ] **Step 5: Commit the promotional article**

```powershell
git add docs/PROMOTION.zh-en.md
git commit -m "docs: add bilingual beta6 announcement"
```
