# Sagen

**Sagen** _(German for "to say")_ is my attempt at making a text-to-speech engine aimed at .NET developers who don't have thousands of dollars at their disposal to license a commercial speech synthesis solution. It is in many ways an experiment and continual learning experience, and I am in no way an expert on speech science, phonetics, or vocal acoustics. I'm just someone crazy enough to try something like this.

## Rationale

Some might ask me, "Why bother?" After all, there are tons of TTS engines out there already.

Short answer: I'm just not happy with what's out there.

Aside from often being prohibitively expensive, commercial TTS technologies are frequently restrictive in their available customization options for voices, voice parameters, and context-sensitive vocal qualities like intonation, stress, and timbre which are necessary to convey emotion and intent. Concatenative synthesizers and other similar "realistic" TTS technologies based on databases of recorded samples also leave a large memory footprint, are relatively slow, and require each voice to be installed separately. While all TTS libraries have advantages and disadvantages, I felt like another alternative was needed.

My goal with this project is not _necessarily_ to produce "something better", but to instead offer the most possible configurability, flexibility, and performance in a formant synthesizer of decent quality. _For free._

## What's planned

Here is a short list of major features that will be supported:
* Text-to-speech based on [formant synthesis](https://en.wikipedia.org/wiki/Speech_synthesis#Formant_synthesis) and physically-based vocal filtering
* Plentiful parameters for tuning how voices sound (age, sex, vocal force, hoarseness, etc...)
* Support for sending speech directly to a WAV file or stream
* Multiple options for sample format and rate
* Support for both [IPA](https://en.wikipedia.org/wiki/International_Phonetic_Alphabet) and [X-SAMPA](https://en.wikipedia.org/wiki/X-SAMPA) pronunciation dictionaries
* Multiple language support (English and German are currently prioritized)
* [Heteronym](https://en.wikipedia.org/wiki/Heteronym_(linguistics)) resolution
* Singing

It is currently a heavy work-in-progress, and I welcome your input and/or contributions.

## Licensing

This project is made available under the MIT License and is completely free for anyone to use, for any purpose, without the burdens of licensing costs or royalties.
